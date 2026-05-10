import '../../core/api/mobile_api_client.dart';

MobileRoleWorkspace gateAccessWorkspace() => MobileRoleWorkspace.demo.firstWhere((workspace) => workspace.roleCode == 'gate_access');
