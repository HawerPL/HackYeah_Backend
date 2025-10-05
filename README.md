# HackYeah Backend

HackYeah Backend to aplikacja napisana w **.NET 8**, udostępniana jako API. Projekt można kompilować, testować i uruchamiać lokalnie, a także budować w postaci kontenera Docker.

---

## Wymagania wstępne

* .NET 8 SDK
* Docker

---

## Kompilacja i testy

Aby skompilować projekt i uruchomić testy jednostkowe lokalnie:

```bash
dotnet restore
dotnet build
dotnet test
```

Publikacja w trybie **Release**:

```bash
dotnet publish -c Release -o ./out
```

---

## Uruchomienie lokalne

Po publikacji możesz uruchomić aplikację lokalnie:

```bash
dotnet ./out/HackYeah_Backend.dll
```

Aplikacja domyślnie nasłuchuje na porcie `8080`.

---

## Budowa obrazu Docker

Obraz składa się z dwóch etapów:

* **build** – kompilacja i publikacja aplikacji
* **runtime** – lekki kontener z .NET ASP.NET Runtime

Aby zbudować obraz:

```bash
docker build -t hackyeah-backend .
```

---

## Uruchomienie w kontenerze

Po zbudowaniu obrazu możesz uruchomić aplikację w kontenerze:

```bash
docker run -d -p 8080:8080 --name hackyeah-backend hackyeah-backend
```

Aplikacja będzie dostępna pod adresem:

```
http://localhost:8080
```
