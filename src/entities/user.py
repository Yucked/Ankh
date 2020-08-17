import json
from entities.badge import Badge

class User:    
    def __init__(self, profile):
        profile = json.loads(profile)
        self.username = profile['avname']
        self.avatar = profile['avpic_url']
        self.id = profile['cid']
        self.gender = profile['gender']
        self.age = profile['age']


        self.registered = profile['registered']
        self.last_login = profile['last_login']

        self.tagline = profile['tagline']

        self.badges = []
        for badge_json in profile['badge_layout']:
            badge = Badge(profile['badge_layout'][badge_json])
            self.badges.append(badge)
