import type { ReactNode } from "react";

export const metadata = {
  title: "SafeSchool Command Center",
  description: "Role-based school NFC, attendance, transport, wallet, mobile, and operations demo",
};

export const dynamic = "force-dynamic";

export default function RootLayout({ children }: { children: ReactNode }) {
  return (
    <html lang="en">
      <body
        style={{
          margin: 0,
          fontFamily:
            'Inter, ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif',
          background: "#f8fafc",
          color: "#172033",
        }}
      >
        {children}
      </body>
    </html>
  );
}
