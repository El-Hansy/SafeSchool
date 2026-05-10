import 'package:flutter/material.dart';

class GateScanScreen extends StatelessWidget {
  const GateScanScreen({super.key, required this.offline});

  final bool offline;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Gate scan')),
      body: Center(child: Text(offline ? 'Offline queue active' : 'Ready to scan')),
    );
  }
}

