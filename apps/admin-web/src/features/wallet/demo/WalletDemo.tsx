import type { ReactNode } from "react";
import { formatMoney, walletDemoData } from "../api/client";
import { guardianWalletDemoData } from "../../guardian-wallet/api/client";

type WalletSection = "overview" | "top-ups" | "pos" | "limits" | "settings" | "review" | "reconciliation" | "transactions";

const schoolTabs: Array<[WalletSection, string, string]> = [
  ["overview", "Overview", "/wallet"],
  ["top-ups", "Top-ups", "/wallet/top-ups"],
  ["pos", "POS", "/wallet/pos"],
  ["limits", "Limits", "/wallet/limits"],
  ["transactions", "History", "/wallet/review"],
  ["settings", "Settings", "/wallet/settings"],
  ["reconciliation", "Reconciliation", "/wallet/reconciliation"],
];

export function WalletDemo({ section = "overview" }: { section?: WalletSection }) {
  return (
    <main style={{ minHeight: "100vh", background: "#eef3f7", color: "#111827", padding: "32px" }}>
      <section style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", gap: 24, marginBottom: 24 }}>
        <div>
          <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>SafeSchool 005</div>
          <h1 style={{ fontSize: 44, lineHeight: 1.05, margin: "8px 0" }}>Wallet Command Center</h1>
          <p style={{ maxWidth: 920, color: "#4b5563", fontSize: 20, lineHeight: 1.45, margin: 0 }}>
            School finance and canteen operations for student wallets, top-ups, POS purchases, limits, corrections, chargebacks, reconciliation, and audit evidence.
          </p>
        </div>
        <div style={{ border: "1px solid #a7f3d0", background: "#dcfce7", color: "#166534", padding: "12px 18px", borderRadius: 6, fontWeight: 800 }}>Demo data</div>
      </section>

      <nav style={{ display: "flex", gap: 10, flexWrap: "wrap", marginBottom: 24 }}>
        {schoolTabs.map(([key, label, href]) => (
          <a key={key} href={href} style={{ textDecoration: "none", color: section === key ? "#1d4ed8" : "#1f2937", border: `1px solid ${section === key ? "#3b82f6" : "#cbd5e1"}`, background: section === key ? "#eff6ff" : "#ffffff", borderRadius: 6, padding: "10px 16px", fontWeight: 800 }}>{label}</a>
        ))}
      </nav>

      <section style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(220px, 1fr))", gap: 16, marginBottom: 24 }}>
        <Metric label="Active wallets" value="128" detail="30 bulk activated in review" />
        <Metric label="Spendable balance" value={formatMoney(183420)} detail="Confirmed credits only" />
        <Metric label="Top-ups today" value="42" detail="2 duplicate retries deduped" />
        <Metric label="POS purchases" value="316" detail="7 held for review" />
        <Metric label="Open exceptions" value="9" detail="Chargeback and reserve review" />
      </section>

      <section style={{ display: "grid", gridTemplateColumns: "minmax(320px, 1.2fr) minmax(320px, 1fr)", gap: 18 }}>
        <Panel title={panelTitle(section)}>
          {section === "pos" ? <PurchaseList /> : section === "top-ups" ? <TopUpList /> : section === "reconciliation" ? <ReconciliationPanel /> : <WalletList />}
        </Panel>
        <Panel title="Controls and boundaries">
          <dl style={{ display: "grid", gap: 14, margin: 0 }}>
            <Boundary title="No payment credentials" detail="Only safe provider references and normalized event ids are shown." />
            <Boundary title="No attendance mutation" detail="Wallet NFC/QR scans do not create attendance or campus access outcomes." />
            <Boundary title="No transport outcome" detail="Canteen purchase scans do not update routes, trips, ETA, or bus status." />
            <Boundary title="Audit required" detail="Sensitive wallet mutations fail when audit evidence cannot be recorded." />
          </dl>
        </Panel>
      </section>
    </main>
  );
}

export function GuardianWalletDemo({ section = "overview" }: { section?: "overview" | "top-ups" | "transactions" | "limits" }) {
  return (
    <main style={{ minHeight: "100vh", background: "#f6f7fb", color: "#111827", padding: "32px" }}>
      <div style={{ fontSize: 13, fontWeight: 800, color: "#5b6472", textTransform: "uppercase" }}>Guardian wallet</div>
      <h1 style={{ fontSize: 42, margin: "8px 0" }}>Linked Student Wallets</h1>
      <p style={{ maxWidth: 800, color: "#4b5563", fontSize: 19, lineHeight: 1.45 }}>Guardian-scoped balance, top-up, spending limit, and transaction history views. Staff-only terminal, settlement, and review assignment fields stay hidden.</p>
      <nav style={{ display: "flex", gap: 10, flexWrap: "wrap", margin: "24px 0" }}>
        {[ ["overview", "/wallet"], ["top-ups", "/wallet/top-ups"], ["transactions", "/wallet/transactions"], ["limits", "/wallet/limits"] ].map(([key, href]) => (
          <a key={key} href={href} style={{ textTransform: "capitalize", textDecoration: "none", border: `1px solid ${section === key ? "#3b82f6" : "#cbd5e1"}`, background: section === key ? "#eff6ff" : "#fff", borderRadius: 6, padding: "10px 16px", fontWeight: 800, color: section === key ? "#1d4ed8" : "#1f2937" }}>{key}</a>
        ))}
      </nav>
      <section style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(260px, 1fr))", gap: 16 }}>
        {guardianWalletDemoData.map((wallet) => <Metric key={wallet.studentProfileId} label={wallet.student} value={formatMoney(wallet.balanceMinor)} detail={`${wallet.issueCount} visible issue${wallet.issueCount === 1 ? "" : "s"}`} />)}
      </section>
    </main>
  );
}

function Metric({ label, value, detail }: { label: string; value: string; detail: string }) {
  return <article style={{ background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 18, boxShadow: "0 1px 2px rgba(15,23,42,.08)" }}><div style={{ fontSize: 34, fontWeight: 900 }}>{value}</div><div style={{ fontWeight: 800 }}>{label}</div><div style={{ color: "#64748b", marginTop: 8 }}>{detail}</div></article>;
}

function Panel({ title, children }: { title: string; children: ReactNode }) {
  return <article style={{ background: "#fff", border: "1px solid #d5dee8", borderRadius: 8, padding: 22, boxShadow: "0 1px 2px rgba(15,23,42,.08)" }}><h2 style={{ marginTop: 0, fontSize: 24 }}>{title}</h2>{children}</article>;
}

function Boundary({ title, detail }: { title: string; detail: string }) {
  return <div><dt style={{ fontWeight: 900 }}>{title}</dt><dd style={{ margin: "4px 0 0", color: "#64748b" }}>{detail}</dd></div>;
}

function WalletList() {
  return <div style={{ display: "grid", gap: 10 }}>{walletDemoData.wallets.map((wallet) => <Row key={wallet.walletId} left={wallet.studentProfileId} middle={formatMoney(wallet.availableBalanceMinor, wallet.currencyCode)} right={wallet.walletStatus} />)}</div>;
}

function TopUpList() {
  return <div style={{ display: "grid", gap: 10 }}>{walletDemoData.topUps.map((topUp) => <Row key={topUp.topUpId} left={topUp.safeReference} middle={formatMoney(topUp.amountMinor, topUp.currencyCode)} right={`${topUp.status} - ${topUp.source}`} />)}</div>;
}

function PurchaseList() {
  return <div style={{ display: "grid", gap: 10 }}>{walletDemoData.purchases.map((purchase) => <Row key={purchase.purchaseId} left={purchase.clientPurchaseId} middle={purchase.decisionReason} right={`${formatMoney(purchase.amountMinor, purchase.currencyCode)} - ${purchase.decision}`} />)}</div>;
}

function ReconciliationPanel() {
  return <div style={{ display: "grid", gap: 10 }}><Row left="Daily run" middle="Ledger vs payments" right="Matched" /><Row left="Offline batch BATCH-18" middle="Reserve review" right="Held" /><Row left="Chargeback PAY-1442" middle="Recovery review" right="Open" /></div>;
}

function Row({ left, middle, right }: { left: string; middle: string; right: string }) {
  return <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr 1fr", gap: 10, alignItems: "center", borderBottom: "1px solid #e5e7eb", padding: "10px 0" }}><strong>{left}</strong><span style={{ color: "#4b5563" }}>{middle}</span><span style={{ color: "#1f2937", fontWeight: 700 }}>{right}</span></div>;
}

function panelTitle(section: WalletSection) {
  const titles: Record<WalletSection, string> = { overview: "Wallet ledger", "top-ups": "Top-up and payment confirmation", pos: "Canteen POS", limits: "Spending limits", settings: "Wallet rule settings", review: "History and corrections", reconciliation: "Reconciliation", transactions: "Transaction history" };
  return titles[section];
}
