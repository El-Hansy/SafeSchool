import '../../core/auth/mobile_auth_context.dart';

class MobileSignInShell {
  MobileAuthContext signInAs(String roleCode, {String languageCode = 'en'}) {
    return MobileAuthContext(
      tenantId: 'school-demo',
      roleCode: roleCode,
      userId: '$roleCode-demo',
      languageCode: languageCode,
    );
  }
}
