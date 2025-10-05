#!/usr/bin/env python3
"""
Convert JSON data to GeoJSON format.

Usage:
    python json_to_geojson.py input.json output.geojson

Supports:
- Single JSON object or list of objects
- Coordinate fields named '@lat', '@lon', 'lat', 'lon', 'latitude', 'longitude', 'x', 'y'
"""

import json
import sys
from typing import Any, Dict, List, Optional, Tuple


# ---------------------------------------------------------
# Helper functions
# ---------------------------------------------------------

def extract_lat_lon(record: Dict[str, Any]) -> Tuple[Optional[float], Optional[float], Dict[str, Any]]:
    """Extract latitude and longitude from a record, return (lat, lon, properties)."""
    lat_keys = ["@lat", "lat", "latitude", "y"]
    lon_keys = ["@lon", "lon", "longitude", "lng", "x"]

    props = dict(record)
    lat = lon = None

    for key in lat_keys:
        if key in props:
            try:
                lat = float(props.pop(key))
            except (ValueError, TypeError):
                lat = None
            break

    for key in lon_keys:
        if key in props:
            try:
                lon = float(props.pop(key))
            except (ValueError, TypeError):
                lon = None
            break

    return lat, lon, props


def record_to_feature(record: Dict[str, Any]) -> Optional[Dict[str, Any]]:
    """Convert a JSON object into a GeoJSON feature."""
    lat, lon, props = extract_lat_lon(record)
    if lat is None or lon is None:
        return None  # skip if missing coordinates

    return {
        "type": "Feature",
        "geometry": {"type": "Point", "coordinates": [lon, lat]},  # GeoJSON: [lon, lat]
        "properties": props,
    }


def convert_json_to_geojson(data: Any) -> Dict[str, Any]:
    """Convert JSON (object or list) to GeoJSON FeatureCollection."""
    features: List[Dict[str, Any]] = []

    if isinstance(data, dict):
        f = record_to_feature(data)
        if f:
            features.append(f)
    elif isinstance(data, list):
        for item in data:
            if isinstance(item, dict):
                f = record_to_feature(item)
                if f:
                    features.append(f)
    else:
        raise ValueError("Input JSON must be an object or an array of objects")

    return {"type": "FeatureCollection", "features": features}


# ---------------------------------------------------------
# Main entry point
# ---------------------------------------------------------

def main():
    if len(sys.argv) != 3:
        print("Usage: python json_to_geojson.py input.json output.geojson")
        sys.exit(1)

    input_file, output_file = sys.argv[1], sys.argv[2]

    with open(input_file, "r", encoding="utf-8") as f:
        data = json.load(f)

    geojson = convert_json_to_geojson(data)

    with open(output_file, "w", encoding="utf-8") as f:
        json.dump(geojson, f, ensure_ascii=False, indent=2)

    print(f"✅ Converted '{input_file}' → '{output_file}' successfully!")


if __name__ == "__main__":
    main()
