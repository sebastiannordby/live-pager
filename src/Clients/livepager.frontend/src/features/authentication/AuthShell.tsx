import { Outlet } from "react-router-dom";
import LivePagerLogo from "../../common/liverpagerlogo";

export default function AuthShell() {
  return (
    <div className="h-full min-h-screen rescue-background">
      <div className="items-center justify-center flex flex-col h-full w-full backdrop-blur-[2px] space-y-6">
        <div className="bg-white flex flex-col p-6 rounded-lg items-center shadow-lg transform transition-transform duration-300 w-full max-w-md">
          <LivePagerLogo className="w-36 mb-4" />

          <hr className="h-[1px] w-full mb-3" />

          <main className="p-2">
            <Outlet />
          </main>
        </div>
      </div>
    </div>
  );
}
