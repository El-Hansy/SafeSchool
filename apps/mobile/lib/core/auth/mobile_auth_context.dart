class MobileAuthContext {
  const MobileAuthContext({
    required this.tenantId,
    required this.roleCode,
    required this.userId,
    required this.languageCode,
  });

  final String tenantId;
  final String roleCode;
  final String userId;
  final String languageCode;

  bool get hasMobileAccess => tenantId.isNotEmpty && roleCode.isNotEmpty;

  MobileAuthContext switchRole(String nextRole) => MobileAuthContext(
        tenantId: tenantId,
        roleCode: nextRole,
        userId: userId,
        languageCode: languageCode,
      );
}
