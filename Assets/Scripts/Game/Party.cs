using System.Collections.Generic;

namespace Anais {

    public class Party {

        private List<Member> members;

        public Member Leader { get; private set; }

        public Party() {
            Reset();
            InitDebug();
        }

        public void Reset() {
            Leader = null;
            members = new List<Member>();
        }

        private void InitDebug() {
            Member member = new Member(new Stats());
            Leader = member;
            members.Add(member);
        }

    }

}
