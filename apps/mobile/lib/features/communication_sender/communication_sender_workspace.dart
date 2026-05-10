import '../../core/api/mobile_api_client.dart';

MobileRoleWorkspace communicationSenderWorkspace() => MobileRoleWorkspace.demo.firstWhere((workspace) => workspace.roleCode == 'communication_sender');
