docker run --name mdnotes_db -v mdnotes_vol:/var/lib/postgres/data -p 6543:5432 --restart unless-stopped -d mdnotes_db
