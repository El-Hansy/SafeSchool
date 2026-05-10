import '../../core/api/mobile_api_client.dart';

MobileRoleWorkspace platformSupportWorkspace() => MobileRoleWorkspace.demo.firstWhere((workspace) => workspace.roleCode == 'platform_support');
