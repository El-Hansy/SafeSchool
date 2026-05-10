import '../../core/api/mobile_api_client.dart';

MobileRoleWorkspace schoolAdminWorkspace() => MobileRoleWorkspace.demo.firstWhere((workspace) => workspace.roleCode == 'school_administrator');
