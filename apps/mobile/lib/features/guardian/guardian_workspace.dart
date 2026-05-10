import '../../core/api/mobile_api_client.dart';

MobileRoleWorkspace guardianWorkspace() => MobileRoleWorkspace.demo.firstWhere((workspace) => workspace.roleCode == 'guardian');
