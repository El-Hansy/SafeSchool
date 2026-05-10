import 'package:flutter/material.dart';

class GuardianSummaryCards extends StatelessWidget {
  const GuardianSummaryCards({super.key, required this.modules});
  final List<String> modules;

  @override
  Widget build(BuildContext context) {
    return Column(children: modules.map((module) => Card(child: ListTile(title: Text(module)))).toList());
  }
}
