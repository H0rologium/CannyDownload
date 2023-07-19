import urllib.request, json, os ,os.path, sys
#Modified https://stackoverflow.com/questions/58474211/github-api-get-body-for-all-new-releases-in-python
author='H0rologium'
repo='CannyDownload'

def applyUpdate():
    os.rename('version.txt','update.txt')
    return



def main(workingDir):
    os.chdir(workingDir)
    if not os.path.isfile("version.txt"):
        print("versioning file not found, not checking for updates")
        return
    f = open("version.txt","r")
    versionnumber = f.read()
    versionnumber = versionnumber.strip()
    f.close()
    release = json.loads(urllib.request.urlopen(f'https://api.github.com/repos/{author}/{repo}/releases?per_page=5').read().decode())
    releases = [
        (data['tag_name'],data['body'])
        for data in release
        if data['tag_name'] > versionnumber][::-1]
    for release in releases:
        if len(release[0]) > 0:
            print("A newer version of the program exists")
            applyUpdate()
            break
    return

if __name__ == '__main__':
    a = sys.argv[1]
    main(a)
