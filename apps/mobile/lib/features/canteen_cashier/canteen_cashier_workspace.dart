import '../../core/api/mobile_api_client.dart';

MobileRoleWorkspace canteenCashierWorkspace() => MobileRoleWorkspace.demo.firstWhere((workspace) => workspace.roleCode == 'canteen_cashier');
