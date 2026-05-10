import '../../core/api/mobile_api_client.dart';

MobileRoleWorkspace studentWorkspace() => MobileRoleWorkspace.demo.firstWhere((workspace) => workspace.roleCode == 'student');
