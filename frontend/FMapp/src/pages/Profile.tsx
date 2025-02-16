import { useAuthStore } from "../store/auth";

export default function Profile() {
  return (
    <div>
      <h2>Личный кабинет</h2>
      <p>Email: user@example.com</p>
      <button onClick={() => useAuthStore.getState().signOut()}>Выйти</button>
    </div>
  );
}
