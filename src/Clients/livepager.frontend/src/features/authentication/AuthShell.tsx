import { Outlet, Link } from "react-router-dom";

export default function AuthShell() {
  return (
    <div
      className="h-full min-h-screen items-center justify-center flex flex-col text-white"
      style={{
        backgroundImage: "url('/images/meteor.svg')",
      }}
    >
      <header className="mb-4">
        <Link to="/" className="text-3xl text-white drop-shadow-md">
          Live Pager
        </Link>
      </header>
      <main className="p-4 bg-white rounded-lg shadow-lg transform transition-transform duration-300">
        <Outlet />
      </main>
    </div>
  );
}
