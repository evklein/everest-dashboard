# Note: This script is meant to be run manually, and any updates should either be placed in some sort of data source,
# or in the source code for the EmojiStringParser utility class.
# Also: this script purposefully nixes all Emojis with duplicate shortnames. This is due to the data source being faulty,
# as many shortnames are reused between images.

import requests

url = "https://gist.githubusercontent.com/oliveratgithub/0bf11a9aff0d6da7b46f1490f86a71eb/raw/d8e4b78cfe66862cf3809443c1dba017f37b61db/emojis.json"

response = requests.request("GET", url)
json = response.json()
emojis = json["emojis"]

dictionary = {}

for i in range(0, len(emojis)):
	shortname = emojis[i]["shortname"]
	emoji = emojis[i]["emoji"]
	dictionary[shortname] = emoji

for key, value in dictionary.items():
	print("{ \"" + key + "\", \"" + value + "\" },")
