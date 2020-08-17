from flask import Flask, render_template, request
import imvu
from entities.user import User

app = Flask(__name__,
            static_url_path='', 
            static_folder='_static',
            template_folder='_templates')

@app.errorhandler(404)
def page_not_found(e):    
    return render_template('404.html'), 404

@app.route('/')
def index():
    return render_template('index.html')

@app.route('/search', methods=['POST'])
def search():
    if request.method != 'POST':
        abort(404, description="Resource not found")
    
    username = request.form['username']
    if request.form.get('profile') != None and request.form.get('profile') == 'on':
        profile = imvu.get_profile(username)

    if request.form.get('privateRooms') != None and request.form['privateRooms'] == 'on':
        privateRooms = imvu.get_privates(username)

    if request.form.get('roomHistory') != None and request.form['roomHistory'] == 'on':
        roomHistory = imvu.get_room_history(username)

    user = User(profile)
    return render_template('info.html', user=user)

    
if __name__ == '__main__':
    app.run(debug=True, host='0.0.0.0')
