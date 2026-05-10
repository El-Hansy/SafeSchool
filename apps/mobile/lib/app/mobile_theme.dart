import 'package:flutter/material.dart';

ThemeData buildSafeSchoolTheme(TextDirection direction) {
  return ThemeData(
    colorScheme: ColorScheme.fromSeed(seedColor: const Color(0xFF2563EB)),
    useMaterial3: true,
    visualDensity: VisualDensity.standard,
    extensions: <ThemeExtension<dynamic>>[
      MobileDirectionTheme(direction),
    ],
  );
}

class MobileDirectionTheme extends ThemeExtension<MobileDirectionTheme> {
  const MobileDirectionTheme(this.textDirection);
  final TextDirection textDirection;

  @override
  MobileDirectionTheme copyWith({TextDirection? textDirection}) =>
      MobileDirectionTheme(textDirection ?? this.textDirection);

  @override
  MobileDirectionTheme lerp(ThemeExtension<MobileDirectionTheme>? other, double t) {
    if (other is! MobileDirectionTheme) {
      return this;
    }
    return t < 0.5 ? this : other;
  }
}
