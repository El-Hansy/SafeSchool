import '../../core/api/mobile_api_client.dart';

MobileRoleWorkspace complaintHandlerWorkspace() => MobileRoleWorkspace.demo.firstWhere((workspace) => workspace.roleCode == 'complaint_handler');
