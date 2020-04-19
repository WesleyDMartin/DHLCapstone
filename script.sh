cd ~/DHLCapstone/
git pull
cd AdminPortal
npm run build
rm -rf /var/www/html/*
cp -a ./dist/* /var/www/htmlt