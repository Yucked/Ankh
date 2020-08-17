import psycopg2

class DB:
    ROOM_PREFIX = 'VUR_{}'
    USER_PREFIX = 'VUU_{}'

    ### queue


    ### setup postgres
    post_conn = psycopg2.connect("dbname=ankh user=postgres password=postgres")


    ### setup redis
    red_conn = redis.Redis(host='localhost', port=6379, db=0)


    def get(key):
        return None

    def remove(key):
        return None