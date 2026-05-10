import '../../core/api/mobile_api_client.dart';

MobileRoleWorkspace medicalStaffWorkspace() => MobileRoleWorkspace.demo.firstWhere((workspace) => workspace.roleCode == 'medical_staff');
