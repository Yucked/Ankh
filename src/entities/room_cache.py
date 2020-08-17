class RoomCache:
    def __init__(room, users):
        self.room = room
        self.users = users

class UserCache:
    def __init__(user, seen):
        self.user = user
        self.last_seen = seen
