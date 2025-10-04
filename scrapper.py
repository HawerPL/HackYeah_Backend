import requests
from bs4 import BeautifulSoup
import json
import time
from urllib.parse import urljoin

BASE_URL = "https://www.gov.pl/web/rcb/komunikaty?page="

all_data = []

for page in range(2, 66):  # pages 1–65
    url = f"{BASE_URL}{page}"
    print(f"📄 Scraping page {page} -> {url}")
    
    response = requests.get(url)
    response.encoding = 'utf-8'
    if response.status_code != 200:
        print(f"⚠️ Failed to load page {page}")
        continue

    soup = BeautifulSoup(response.text, 'html.parser')

    events = soup.find_all('div', class_='event')
    titles = soup.find_all('div', class_='title')
    intros = soup.find_all('div', class_='intro')
    
    all_images = soup.find_all('img')
    content_images = []
    
    for img in all_images:
        img_src = img.get('src')
        if img_src:
            # Convert relative URL to absolute URL
            img_url = urljoin(url, img_src)
            
            # Filter out common non-content images (logos, icons, etc.)
            if not any(ignore in img_url.lower() for ignore in ['logo', 'icon', 'spacer', 'pixel', 'placeholder']):
                content_images.append(img_url)
    
    print(f"📸 Found {len(content_images)} content images on page {page}")
    
    
    for index, (e, t, i) in enumerate(zip(events, titles, intros)):
        image_url = None
        
        
        image_index = index + 2  # 1st article gets 3rd image (index 2), 2nd gets 4th (index 3), etc.
        
        if image_index < len(content_images):
            image_url = content_images[image_index]
            print(f"🖼️ Assigning image {image_index + 1} to article {index + 1}")
        else:
            print(f"⚠️ No image available for article {index + 1}")
        
        all_data.append({
            "event": e.get_text(strip=True),
            "title": t.get_text(strip=True),
            "intro": i.get_text(strip=True),
            "image_url": image_url
        })

# Save everything into one JSON file
with open("komunikaty.json", "w", encoding="utf-8") as f:
    json.dump(all_data, f, ensure_ascii=False, indent=4)

print(f"✅ Scraped {len(all_data)} entries from 65 pages and saved to komunikaty.json")
print(f"📊 Image assignment: 3rd photo → 1st article, 4th photo → 2nd article, etc.")