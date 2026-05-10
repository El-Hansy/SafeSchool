import '../../core/api/mobile_api_client.dart';

MobileRoleWorkspace teacherWorkspace() => MobileRoleWorkspace.demo.firstWhere((workspace) => workspace.roleCode == 'teacher');
