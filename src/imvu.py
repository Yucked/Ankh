import requests
import logging

def get_profile(username):
    cid = username if username.isnumeric() else get_cid(username)
    response = requests.get(f'http://www.imvu.com/api/avatarcard.php?cid={cid}')
    
    ## Check for status_code

    return response.text

def get_cid(username):
    request_body = f"""<?xml version="1.0" encoding="UTF-8"?>
<methodCall>
<methodName>gateway.getUserIdForAvatarName</methodName>
<params>
    <param>
        <value>
            <string>{username}</string>
        </value>
    </param>
</params>
</methodCall>"""
    response = requests.post(
        'http://secure.imvu.com//catalog/skudb/gateway.php',
        data = request_body,
        headers = {'Content-Type': 'application/xml'})

    return response.text[106:response.text.index('</int>')]

def get_privates(username):
    return username

def get_room_history(username):
    return username