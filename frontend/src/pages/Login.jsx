import { Link } from 'react-router-dom';
import LoginForm from '../features/auth/components/LoginForm';

const Login = () => {
  return (
    <div className="min-h-screen bg-slate-100 px-4 py-10">
      <div className="mx-auto grid max-w-6xl gap-8 lg:grid-cols-[1.1fr_0.9fr]">
        <section className="rounded-3xl bg-slate-900 p-8 text-white shadow-2xl lg:p-12">
          <p className="mb-4 inline-flex rounded-full bg-white/10 px-4 py-1 text-sm font-medium text-slate-200">
            City Transit Pass System
          </p>
          <h1 className="max-w-lg text-4xl font-black leading-tight">
            Travel smarter with one pass for bus, metro, and city routes.
          </h1>
          <p className="mt-5 max-w-xl text-base leading-7 text-slate-300">
            Sign in to view active passes, recent journeys, and commuter tools.
          </p>
        </section>

        <section className="rounded-3xl bg-white p-8 shadow-xl">
          <div className="mb-6">
            <h2 className="text-3xl font-bold text-slate-900">Login</h2>
            <p className="mt-2 text-sm text-slate-600">
              Use your account to continue.
            </p>
          </div>

          <LoginForm />

          <p className="mt-6 text-sm text-slate-600">
            New here?{' '}
            <Link className="font-semibold text-sky-700 hover:text-sky-800" to="/register">
              Create an account
            </Link>
          </p>
        </section>
      </div>
    </div>
  );
};

export default Login;
