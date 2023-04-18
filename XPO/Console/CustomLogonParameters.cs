using System.ComponentModel;
using System.Runtime.Serialization;
using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;

namespace ConsoleApplication;

public class CustomLogonParameters : INotifyPropertyChanged, ISerializable
{
    private PermissionPolicyUser _user;
    private string _password;
    
    public PermissionPolicyUser User
    {
        get => _user;
        set
        {
            if (value == _user) return;
            _user = value;
            UserName = _user.UserName;
            OnPropertyChanged(nameof(User));
        }
    }
    
    public string UserName { get; set; }
    
    public string Password
    {
        get { return _password; }
        set
        {
            if (_password == value) return;
            _password = value;
        }
    }
    
    public string Tenant { get; set; }

    public CustomLogonParameters()
    {
    }

    // ISerializable 
    public CustomLogonParameters(SerializationInfo info, StreamingContext context)
    {
        if (info.MemberCount > 0)
        {
            UserName = info.GetString("UserName");
            Password = info.GetString("Password");
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [System.Security.SecurityCritical]
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("UserName", UserName);
        info.AddValue("Password", Password);
    }

    public void RefreshPersistentObjects(IObjectSpace objectSpace)
    {
        User = (UserName == null) ? null : objectSpace.FirstOrDefault<PermissionPolicyUser>(e => e.UserName == UserName);
    }
}