import 'package:flutter/material.dart';
import 'wallet_pos_models.dart';

class WalletPosScreen extends StatelessWidget {
  const WalletPosScreen({super.key, required this.terminalCode, required this.lastDecision});

  final String terminalCode;
  final WalletPosDecision lastDecision;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text('Wallet POS $terminalCode')),
      body: Center(child: Text('Last wallet decision: ${lastDecision.name}')),
    );
  }
}
