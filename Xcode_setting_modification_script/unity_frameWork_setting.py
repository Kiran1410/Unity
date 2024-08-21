
import os
import shutil


from pbxproj import PBXGenericObject, XcodeProject
from pbxproj.pbxextensions.ProjectFiles import FileOptions


class UnityFrameWorkSetting:
    __project_base_path = f'{os.getcwd().replace("/Xcode_setting_modification_script","")}'
    __path = f'{__project_base_path}/unity_ios_export_project/Unity-iPhone.xcodeproj/project.pbxproj'
    __assettag_settings   = 'settings'
    # __assettag_name       = 'ASSET_TAGS'

    def __init__(self):
        print(self.__path)
        self.__project = XcodeProject.load(self.__path)

    def update_build_setting(self):
        try: #buildSettings
        
            configs = self.__project.get_build_phases_by_name("XCBuildConfiguration")
            newConfig = []
            for config in configs:

                name = config['name']
                buildSettings = config['buildSettings']
                PRODUCT_BUNDLE_IDENTIFIER = buildSettings['PRODUCT_BUNDLE_IDENTIFIER']
                if(PRODUCT_BUNDLE_IDENTIFIER == 'com.unity3d.framework') : #name == 'Release' and 
                     #"$(ARCHS_STANDARD)"
                    buildSettings['ARCHS'] = "$(ARCHS_STANDARD)"
                    buildSettings['SKIP_INSTALL'] = False
                    config["buildSettings"] = buildSettings 
                    print(buildSettings)
                newConfig.append(config)

            PBXGenericObject(self.__project).parse(newConfig)
            self.__project.save()
            print(f'********** Update BuildSettings *******************')

        except Exception as e:
             print(e)

    # def data_folder_target_change(self) :

    #     try :

    #         objectList = self.__project.add_file("Data", tree='<group>', target_name=['UnityFramework'], force=False)
    #         delete_status = self.__project.remove_files_by_path(path="Data", tree="<group>", target_name=["Unity-iPhone"])
    #         print(f'delete status ==>  \n {objectList}')

    #         parse_obj = PBXGenericObject(self.__project).parse(objectList)
    #         dictlist = { self.__assettag_settings : parse_obj}
    #         PBXGenericObject(self.__project).parse(dictlist)
    #         self.__project.save()
    #         print(f'********** Update Data Target *******************')

    #     except Exception as e :
    #         print(e)

    def bridge_File_Target_Public(self) :

        try :

            # Libraries/IVY Foundation/Bridge/Bridge.h
            resourceGroups = self.__project.get_groups_by_path(str("Bridge"))
            print(resourceGroups)

            file_opt = FileOptions()
            file_opt.header_scope = "Public"
            file_opt.add_groups_relative = True
            file_opt.create_build_files = True

            for resource_group in resourceGroups:

                childrens = resource_group["children"]
                print(childrens)

                for file_id in childrens:

                    file_info = self.__project.get_file_by_id(file_id)
                    file_name = str(file_info['name'])
                    print(f'file_name ==> {file_name}')
                    if (file_name == "Bridge.h") :
                        delete_status = self.__project.remove_file_by_id(file_id=str(file_id), target_name='UnityFramework')
                        print(f'delete {file_name} status ==> {delete_status}')
                        print(file_info)
                        self.__project.save()
                        break

            self.remove_file()
            
            objectList = self.__project.add_file("Bridge.h", parent=resourceGroups[0],  tree='<group>', target_name=['UnityFramework'], force=False, file_options=file_opt)
            # print(f'objectList ==> {objectList}')
            # self.__project.save()
            self.update_parse_object_file(objectList=objectList)

            objectList = self.__project.add_file("Data", tree='<group>', target_name=['UnityFramework'], force=True)
            self.update_parse_object_file(objectList=objectList)
            # self.__project.save()
   
            print(f'********** Update Bridge Target *******************')

        except Exception as e:
            print(e)
    
    def remove_file(self) :

        projec_files_id = self.__project.get_ids()        
        for file_id in projec_files_id :
            try :
                file_info = self.__project.get_file_by_id(file_id)
                if (file_info['path'] != None) :
                    file_path = str(file_info['path'])
                    if (file_path == "Data"):
                        delete_status = self.__project.remove_files_by_path(path=file_path,tree="<group>", target_name=["Unity-iPhone", "UnityFramework"])#self.__project.remove_files_by_path(path=file_info['path'], tree=file_info['sourceTree'], target_name='Unity-iPhone')
                        print(f'delete Data status ==> {delete_status}')
                        self.__project.save()
                        print(file_info)
                        return
            except Exception as e:
                # print(e)
                continue
    
    def update_header_files(self) :
        desc_path = f'{self.__project_base_path}/unity_ios_export_project/UnityFramework/UnityFramework.h'
        src_path = f'{os.getcwd()}/swift_files/UnityFramework.h'
        self.copy_file(src=src_path, desc=desc_path)

    def update_main_mm_files(self) :
        desc_path = f'{self.__project_base_path}/unity_ios_export_project/Classes/main.mm'
        src_path = f'{os.getcwd()}/swift_files/main.mm'
        self.copy_file(src=src_path, desc=desc_path)

    def copy_file(self, src: str, desc: str):
        src = src.replace('\\', "")
        desc = desc.replace('\\', "")
        src = src.replace('\\', "")
        desc = desc.replace('\\', "")
        shutil.copy(src,desc)

    def update_parse_object_file(self, objectList):
        print(f'objectList ==> {objectList}')
        for fileObj in objectList:
            parse_obj = PBXGenericObject(self.__project).parse(fileObj)
            fileObj = parse_obj
        self.__project.save()
