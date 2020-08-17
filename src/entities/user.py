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
        self.location = profile['location']
        self.availability = profile['availability']
        self.albums = profile['visible_albums']

        self.registered = profile['registered']
        self.last_login = profile['last_login']
        
        self.tagline = profile['tagline']
        self.interests = None

        self.badge_count = profile['badge_count']
        self.badge_level = profile['badge_level']
        self.badges = []
        for badge_json in profile['badge_layout']:
            badge = Badge(profile['badge_layout'][badge_json])
            self.badges.append(badge)
        
        self.relationship = profile['dating']['relationship_status']
        self.orientation = profile['dating']['orientation']
        self.looking_for = profile['dating']['looking_for']

        self.rooms = []
        if len(profile['public_rooms']) == 0:
            return

        for room_json in profile['public_rooms']:
            room = Room(room_json)
            self.rooms.append(room)