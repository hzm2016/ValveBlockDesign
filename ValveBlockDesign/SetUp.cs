using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Runtime.InteropServices;
using Inventor;
using Microsoft.Win32;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;


namespace ValveBlockDesign
{
    [RunInstaller(true)]
    public partial class SetUp : System.Configuration.Install.Installer
    {
        public SetUp()
        {
            InitializeComponent();
        }
    }
}
