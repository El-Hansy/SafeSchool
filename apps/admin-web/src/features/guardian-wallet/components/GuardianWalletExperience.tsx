import type { ReactNode } from "react";
import { formatMoney } from "../../wallet/api/client";
import { loadGuardianWalletData } from "../api/client";
import { GuardianTopUpAction } from "./GuardianWalletForms";

export type GuardianWalletSection = "overview" | "top-ups" | "transactions" | "limits";

const tabs: Array<[GuardianWalletSection, string, string]> = [
  ["overview", "Overview", "/guardian/wallet"],
  ["top-ups", "Top-ups", "/guardian/wallet/top-ups"],
  ["transactions", "Transactions", "/guardian/wallet/transactions"],
  ["limits", "Limits", "/guardian/wallet/limits"],
];

export async function GuardianWalletExperience({ section = "overview", studentProfileId = "student-amina" }: { section?: GuardianWalletSection; studentProfileId?: string }) {
  const data = await loadGuardianWalletData(studentProfileId);

  return (
    <main style={{ minHeight: "100vh", background: "#f6f7fb", color: "#111827", padding: "32px" }}>
      <div style={{ display: "flex", justifyContent: "space-between", gap: 20, alignItems: "flex-start" }}>
        <div>
          <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>Guardian wallet</div>
          <h1 style={{ fontSize: 42, margin: "8px 0" }}>Linked Student Wallet</h1>
          <p style={{ maxWidth: 800, color: "#4b5563", fontSize: 19, lineHeight: 1.45 }}>Guardian-scoped balance, top-up, spending limit, and transaction history views. Staff-only terminal, settlement, and review assignment fields stay hidden.</p>
        </div>
        <div style={{ border: `1px solid ${data.dataSource === "api" ? "#a7f3d0" : "#fed7aa"}`, background: data.dataSource === "api" ? "#dcfce7" : "#fff7ed", color: data.dataSource === "api" ? "#166534" : "#9a3412", padding: "12px 18px", borderRadius: 6, fontWeight: 800 }}>
          {data.dataSource === "api" ? "API connected" : "API fallback"}
        </div>
      </div>
      <nav style={{ display: "flex", gap: 10, flexWrap: "wrap", margin: "24px 0" }}>
        {tabs.map(([key, label, href]) => (
          <a key={key} href={href} style={{ textDecoration: "none", border: `1px solid ${section === key ? "#3b82f6" : "#cbd5e1"}`, background: section === key ? "#eff6ff" : "#fff", borderRadius: 6, padding: "10px 16px", fontWeight: 800, color: section === key ? "#1d4ed8" : "#1f2937" }}>{label}</a>
        ))}
      </nav>
      <section style={{ display: "grid", gridTemplateColumns: "minmax(280px, .8fr) minmax(320px, 1.2fr)", gap: 18 }}>
        <Panel title="Balance">
          {data.wallet ? (
            <dl style={{ display: "grid", gap: 12, margin: 0 }}>
              <Metric label="Student" value={data.wallet.studentProfileId} />
              <Metric label="Spendable" value={formatMoney(data.wallet.availableBalanceMinor, data.wallet.currencyCode)} />
              <Metric label="Status" value={data.wallet.walletStatus} />
            </dl>
          ) : <p>No wallet is visible for this linked student.</p>}
        </Panel>
        <Panel title={sectionTitle(section)}>
          {section === "top-ups" && data.wallet ? <Stack><GuardianTopUpAction walletId={data.wallet.walletId} studentProfileId={data.studentProfileId} /><Rows rows={data.topUps.map((topUp) => [topUp.safeReference, formatMoney(topUp.amountMinor, topUp.currencyCode), topUp.status])} /></Stack> : null}
          {section === "transactions" ? <Rows rows={data.transactions.map((tx) => [tx.transactionId, formatMoney(tx.amountMinor, tx.currencyCode), tx.merchantOrSource])} /> : null}
          {section === "limits" ? <Rows rows={data.limits.map((limit) => [limit.limitType, formatMoney(limit.amountMinor, limit.currencyCode), limit.status])} /> : null}
          {section === "overview" ? <Rows rows={[["Top-ups", String(data.topUps.length), "guardian-visible"], ["Transactions", String(data.transactions.length), "staff details suppressed"], ["Limits", String(data.limits.length), "guardian controls"]]} /> : null}
        </Panel>
      </section>
    </main>
  );
}

function Panel({ title, children }: { title: string; children: ReactNode }) {
  return <article style={{ background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 22, boxShadow: "0 1px 2px rgba(15,23,42,.08)" }}><h2 style={{ marginTop: 0 }}>{title}</h2>{children}</article>;
}

function Stack({ children }: { children: ReactNode }) {
  return <div style={{ display: "grid", gap: 18 }}>{children}</div>;
}

function Metric({ label, value }: { label: string; value: string }) {
  return <div><dt style={{ color: "#667085", fontWeight: 800 }}>{label}</dt><dd style={{ margin: "4px 0 0", color: "#101828", fontWeight: 900, fontSize: 22 }}>{value}</dd></div>;
}

function Rows({ rows }: { rows: string[][] }) {
  if (rows.length === 0) return <p style={{ color: "#667085", margin: 0 }}>No guardian-visible records returned.</p>;
  return <div style={{ display: "grid", gap: 10 }}>{rows.map((row) => <div key={row.join(":")} style={{ display: "grid", gridTemplateColumns: "1fr 1fr 1fr", gap: 10, borderBottom: "1px solid #e5e7eb", padding: "10px 0" }}>{row.map((cell, index) => <span key={`${cell}-${index}`} style={{ fontWeight: index === 0 ? 800 : 500, color: index === 0 ? "#101828" : "#475467" }}>{cell}</span>)}</div>)}</div>;
}

function sectionTitle(section: GuardianWalletSection) {
  const titles: Record<GuardianWalletSection, string> = { overview: "Visible activity", "top-ups": "Add amount", transactions: "Transaction history", limits: "Spending limits" };
  return titles[section];
}
