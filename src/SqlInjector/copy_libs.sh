#!/bin/bash
set -e

rm -rf wwwroot/lib

copy() {
    mkdir -p wwwroot/lib/$2 && cp -vr node_modules/$1 wwwroot/lib/$2
}

copy alpinejs/dist/* alpinejs
copy dexie/dist/* dexie
copy feather-icons/dist/* feather-icons
copy font-awesome/css/* font-awesome/css
copy font-awesome/fonts/* font-awesome/fonts
copy purecss/build/* purecss
