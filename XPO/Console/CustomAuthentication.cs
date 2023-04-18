using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base.Security;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;

namespace ConsoleApplication;

public class CustomAuthentication : AuthenticationBase, IAuthenticationStandard {
    private CustomLogonParameters customLogonParameters;
    public CustomAuthentication() {
        customLogonParameters = new CustomLogonParameters();
    }
    public override void Logoff() {
        base.Logoff();
        customLogonParameters = new CustomLogonParameters();
    }
    public override void ClearSecuredLogonParameters() {
        customLogonParameters.Password = "";
        base.ClearSecuredLogonParameters();
    }
    public override object Authenticate(IObjectSpace objectSpace) {

        PermissionPolicyUser user = objectSpace.FirstOrDefault<PermissionPolicyUser>(e => e.UserName == customLogonParameters.UserName);
        
        if (user == null)
            throw new ArgumentNullException("User");

        if (!((IAuthenticationStandardUser)user).ComparePassword(customLogonParameters.Password))
            throw new AuthenticationException(
                user.UserName, "Password mismatch.");

        // Some extra logic needs to be checked on login
        // if(user.Tenant == customLogonParameters.Tenant) ...
        
        return user;
    }

    public override void SetLogonParameters(object logonParameters) {
        this.customLogonParameters = (CustomLogonParameters)logonParameters;
    }

    public override IList<Type> GetBusinessClasses() {
        return new Type[] { typeof(CustomLogonParameters) };
    }
    public override bool AskLogonParametersViaUI {
        get { return true; }
    }
    public override object LogonParameters {
        get { return customLogonParameters; }
    }
    public override bool IsLogoffEnabled {
        get { return true; }
    }
}