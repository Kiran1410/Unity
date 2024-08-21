
import os
import sys
import venv
import shutil

def create_env():

    def isvirtualenv():
        return sys.prefix != sys.base_prefix

    def findfile(startdir, pattern):
        for root, dirs, files in os.walk(startdir):
            for name in files:
                if name.find(pattern) >= 0:
                    return root + os.sep + name
        return None

    venv_path = 'env'           
    evn_path = (os.getcwd().replace("/Xcode_setting_modification_script",""))
    os.chdir(evn_path)
    delete_env_dir(path=evn_path)
    if isvirtualenv():
        print('Already in virtual environment.')
    else:
        if findfile(evn_path, 'activate') is None:
            print('No virtual environment found. Creating one.')
            env = venv.EnvBuilder(with_pip = True)
            env.create(venv_path)
        else:
            print('Not in virtual environment. Virtual environment directory found.')
        os.environ['PATH'] = os.path.dirname(findfile(evn_path, 'activate')) + os.pathsep + os.environ['PATH']


    gc_path = f'{os.getcwd()}/Xcode_setting_modification_script/'
    os.chdir(gc_path)
    os.system(f"python3 -m pip install -r requirements.txt")
    os.system(f"python3 main.py")
    
    
def delete_env_dir(path):
    evn_dir = path + f'/env'
    print(f"current evn_dir ==> {evn_dir}")
    if os.path.exists(evn_dir):
        print(f"removing evn_dir ==> {evn_dir}")
        shutil.rmtree(evn_dir)

def main():
    try:
        create_env()
    except Exception as ex:
        print(ex)
        sys.exit(-1)

if __name__ == "__main__":
    main()

 
