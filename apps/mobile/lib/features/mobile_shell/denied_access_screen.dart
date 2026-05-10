import 'package:flutter/material.dart';

class DeniedAccessScreen extends StatelessWidget {
  const DeniedAccessScreen({super.key, required this.reason});
  final String reason;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Access blocked')),
      body: Center(child: Text(reason)),
    );
  }
}
