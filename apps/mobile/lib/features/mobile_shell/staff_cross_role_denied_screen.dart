import 'package:flutter/material.dart';

class StaffCrossRoleDeniedScreen extends StatelessWidget {
  const StaffCrossRoleDeniedScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return const Scaffold(body: Center(child: Text('This staff action is outside the active role.')));
  }
}
