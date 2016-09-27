@echo off
echo Internet access is required to download the conntent, make sure you are connected before proceeding.
echo By proceeding you automaticly accept the terms and conditions of the Flaticon Basic License and accept that it applies for the patched images as well.
echo You will provide the credits to Roundicons and Flaticon for any use the license requires of you in a way that satisfies the terms and conditions by the license.
echo Icons made by Roundicons ( www.flaticon.com/authors/roundicons ) from www.flaticon.com
echo Press any key to accept the terms and proceed the instalation or CTRL+C to terminate the instalation.
pause

echo Downloading...
cd patch
cd wget
::First the get images
::Get add image
wget  --output-document="add.org" --post-data="icon_id=189689&author=201&team=201&keyword=Add&pack=189637&style=0&format=png&color=%23000000&colored=2&size=16&selection=1&premium=0" http://www.flaticon.com/download-icon
::Get modem image
wget  --output-document="modem.org" --post-data="icon_id=189552&author=201&team=201&keyword=Modem&pack=189543&style=0&format=png&color=%23000000&colored=2&size=256&selection=1&premium=0" http://www.flaticon.com/download-icon
::Get move image
wget  --output-document="move.org" --post-data="icon_id=189275&author=201&team=201&keyword=Resize&pack=189238&style=0&format=png&color=%23000000&colored=2&size=32&selection=1&premium=0" http://www.flaticon.com/download-icon
::Get remove image
wget  --output-document="remove.org" --post-data="icon_id=189690&author=201&team=201&keyword=Remove&pack=189637&style=0&format=png&color=%23000000&colored=2&size=16&selection=1&premium=0" http://www.flaticon.com/download-icon
::Get setting image
wget  --output-document="settings.org" --post-data="icon_id=190134&author=201&team=201&keyword=Settings&pack=190090&style=0&format=png&color=%23000000&colored=2&size=16&selection=1&premium=0" http://www.flaticon.com/download-icon

::then patch the images
echo Patching content
cd..

bspatch wget\add.org ..\..\add.png _add.diff
bspatch wget\modem.org ..\..\modem_blue.ico _modem_blue.diff
bspatch wget\modem.org ..\..\modem_green.ico _modem_green.diff
bspatch wget\modem.org ..\..\modem_red.ico _modem_red.diff
bspatch wget\move.org ..\..\move.png _move.diff
bspatch wget\remove.org ..\..\remove.png _remove.diff
bspatch wget\settings.org ..\..\settings.png _settings.diff

::Cleaning up
echo Removing downloaded files

cd wget
del /f /q *org

::done
cd..
cd..
echo installation complete.
echo You may keep or delete this folder.
pause