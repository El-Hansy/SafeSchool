import '../../core/api/mobile_api_client.dart';

class MobileRoleWorkspaceRegistry {
  static List<MobileRoleWorkspace> get all => MobileRoleWorkspace.demo;
  static MobileRoleWorkspace byRole(String roleCode) => all.firstWhere((workspace) => workspace.roleCode == roleCode);
}
