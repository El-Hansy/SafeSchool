import '../../core/api/mobile_api_client.dart';

MobileRoleWorkspace documentAdminWorkspace() => MobileRoleWorkspace.demo.firstWhere((workspace) => workspace.roleCode == 'document_administrator');
