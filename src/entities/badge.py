import json

class Badge:
    def __init__(self, badge):
        # badge = json.loads(badge_json)
        self.name = badge['name']
        self.image = badge['image_url']