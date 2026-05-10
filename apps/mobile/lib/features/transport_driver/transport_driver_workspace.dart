import '../../core/api/mobile_api_client.dart';

MobileRoleWorkspace transportDriverWorkspace() => MobileRoleWorkspace.demo.firstWhere((workspace) => workspace.roleCode == 'transport_driver');
