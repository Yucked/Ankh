import json

class Badge:
    def __init__(self, badge):
        # badge = json.loads(badge_json)
        self.name = badge['name']
        self.image = badge['image_url']
        self.id = badge['badgeid']
        self.badge_type = badge['badge_type']

        self.creator_id = badge['creator_id']
        self.badge_index = badge['creator_badge_index']

        self.is_autogrant = badge['allow_autogrant']
