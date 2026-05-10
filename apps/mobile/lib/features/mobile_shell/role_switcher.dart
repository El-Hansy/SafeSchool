class RoleSwitcher {
  const RoleSwitcher(this.availableRoles);
  final List<String> availableRoles;

  String switchTo(String roleCode) {
    if (!availableRoles.contains(roleCode)) {
      throw StateError('Role is not assigned');
    }
    return roleCode;
  }
}
