﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TabPlayer.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.10.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool Repeat {
            get {
                return ((bool)(this["Repeat"]));
            }
            set {
                this["Repeat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool PauseOnEdit {
            get {
                return ((bool)(this["PauseOnEdit"]));
            }
            set {
                this["PauseOnEdit"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"E |--0-------0---------------0---------------0---------------------|
B |--0---------0-------------0-----------------0-------------------|
G#|--1-----------1---1---3---0-------0-----------0-------0---------|
C#|--0-----0-------------------------0-----------------3-----3-----|
A |--0-----0-------0-----------------x---------------2-----------0-|
E |--0-----0---------------------0---4-----4-------2---------------|

|--0-------0---------------0---------------0---------------------|
|--0---------0-------------0-----------------0-------------------|
|--1-----------1---5---3---0-------0-----------0-------0---------|
|--0-----0-------------------------0-----------------10----10----|
|--0-----0-------0-----------------x---------------9-----------0-|
|--0-----0---------------------0---4---7-7-------7---------------|")]
        public string Tab {
            get {
                return ((string)(this["Tab"]));
            }
            set {
                this["Tab"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("494, 448")]
        public global::System.Drawing.Size Size {
            get {
                return ((global::System.Drawing.Size)(this["Size"]));
            }
            set {
                this["Size"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int Speed {
            get {
                return ((int)(this["Speed"]));
            }
            set {
                this["Speed"] = value;
            }
        }
    }
}
