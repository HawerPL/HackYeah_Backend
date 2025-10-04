# save_as: make_selected_voivodeships_geojson.py
"""
Skrypt: tworzy GeoJSON zawierający tylko wskazane województwa.
Wymagania: geopandas, requests, fiona, pyproj, shapely
Instalacja: pip install geopandas requests fiona pyproj shapely
Uruchomienie: python make_selected_voivodeships_geojson.py
"""

import geopandas as gpd
import requests
import io
import unicodedata

# --- CONFIG: źródło danych ---
GEOJSON_URL = "https://raw.githubusercontent.com/ppatrzyk/polska-geojson/master/wojewodztwa/wojewodztwa-min.geojson"


# --- Lista województw do uwzględnienia ---
wojewodztwa_target = [
    "dolnoslaskie"
]

# --- funkcja do normalizacji nazw ---
def normalize_name(s):
    if s is None:
        return ""
    s = str(s).strip().lower()
    s = ''.join(ch for ch in unicodedata.normalize('NFD', s) if unicodedata.category(ch) != 'Mn')
    return s

# --- 1) Pobierz dane województw ---
print("Pobieram dane z:", GEOJSON_URL)
r = requests.get(GEOJSON_URL)
r.raise_for_status()
data = r.content

gdf = gpd.read_file(io.BytesIO(data))
print("Wczytano", len(gdf), "województw")

# --- 2) Zidentyfikuj kolumnę z nazwami ---
name_col = None
for col in gdf.columns:
    if any(x in col.lower() for x in ("name", "nazwa", "jpt_nazwa", "wojewodztwo")):
        name_col = col
        break
if name_col is None:
    for col in gdf.columns:
        if gdf[col].dtype == object:
            name_col = col
            break

print("Kolumna z nazwami województw:", name_col)

# --- 3) Normalizuj i wybierz tylko wskazane województwa ---
gdf["name_norm"] = gdf[name_col].apply(normalize_name)
targets_norm = [normalize_name(w) for w in wojewodztwa_target]

mask = gdf["name_norm"].isin(targets_norm)
selected_gdf = gdf[mask].copy()
print("Wybrano", len(selected_gdf), "województw.")

if selected_gdf.empty:
    raise SystemExit("Nie znaleziono żadnego dopasowanego województwa. Sprawdź nazwy lub źródło danych.")

# --- 4) Napraw ewentualne błędy topologiczne ---
selected_gdf["geometry"] = selected_gdf["geometry"].buffer(0)
selected_gdf = selected_gdf[selected_gdf.is_valid]

# --- 5) Scal wszystkie województwa w jeden obszar ---
dissolved = selected_gdf.unary_union
dissolved_gdf = gpd.GeoDataFrame(geometry=[dissolved], crs=selected_gdf.crs)

# --- 6) Zapisz wynik ---
outfn = "selected_voivodeships.geojson"
dissolved_gdf.to_file(outfn, driver="GeoJSON")
print("✅ Zapisano wynik do:", outfn)