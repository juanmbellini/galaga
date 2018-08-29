using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public Rigidbody2D body;
    public BoxCollider2D collider;
    public Animator animatorCtrl;

    /// <summary>
    /// Indicates the speed (velocity module) for this enemy.
    /// </summary>
    [SerializeField] private float speed;

    // ========================================================
    // TODO: remove this! This values must be set by the manager through the Spawn method.
    [SerializeField] private Vector3 _startingPoint;

    [SerializeField] private Vector3 _firstStop;

    [SerializeField] private Vector3 _secondStop;

    [SerializeField] private Vector3 _finalPoint;

    [SerializeField] private Player _player;
    // ========================================================

    /// <summary>
    /// A MovementStateMachine that will dictate this enemy's movement.
    /// </summary>
    private MovementStateMachine _movementStateMachine;

    /// <summary>
    /// Constructor. This sets the state machine to null.
    /// </summary>
    public Enemy() {
        _movementStateMachine = null;
    }


    // Use this for initialization
    private void Start() {
        // TODO: remove this! Spawn is called from the enemy manager
        Physics.IgnoreLayerCollision(0, 9);
        Physics.IgnoreLayerCollision(9, 9);
        _player = FindObjectOfType<Player>();
        //Spawn(_startingPoint, _firstStop, _secondStop, _finalPoint);
    }

    // Update is called once per frame
    private void Update() {
        if (_movementStateMachine != null) {
            _movementStateMachine.Execute();
        }
    }

    /// <summary>
    /// Spawns this enemy.
    /// </summary>
    /// <param name="startingPoint">The starting point for this enemy.</param>
    /// <param name="firstStop">The point in which this enemy will start performing the circular movement.</param>
    /// <param name="secondStop">The center of the circle.</param>
    /// <param name="finalPoint">The final point for the enemy (i.e place in the enemy grid).</param>
    public void Spawn(Vector3 startingPoint,
        Vector3 firstStop, Vector3 secondStop,
        Vector3 finalPoint) {
        if (_movementStateMachine != null) {
            return;
        }
        transform.position = startingPoint;
        gameObject.SetActive(true);
        _movementStateMachine =
            new MovementStateMachine(speed, firstStop, secondStop, finalPoint, _player.transform, transform);
    }

    /// <summary>
    /// Indicates whether the enemy is alive.
    /// </summary>
    /// <returns>true if the enemy is alive, or false otherwise.</returns>
    public bool IsAlive() {
        return _movementStateMachine != null;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        animatorCtrl.SetTrigger("Death");
        StartCoroutine(KillEnemy());
    }

    private IEnumerator KillEnemy() {
        _movementStateMachine = null;
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// The state machine that dictates how the enemy moves.
    /// </summary>
    private class MovementStateMachine {
        /// <summary>
        /// The enemy's speed.
        /// </summary>
        private readonly float _speed;

        /// <summary>
        /// The point in which this enemy will start performing the circular movement.
        /// </summary>
        private readonly Vector3 _firstStop;

        /// <summary>
        /// The center of the circle.
        /// </summary>
        private readonly Vector3 _secondStop;


        /// <summary>
        /// The final point for the enemy (i.e place in the enemy grid).
        /// </summary>
        private readonly Vector3 _finalPoint;

        /// <summary>
        /// The player's transform (this is a reference to it, storing it in order to get always the actual position).
        /// </summary>
        private readonly Transform _playerTransform;

        /// <summary>
        /// The enemy's transform (this is a reference to it, storing it in order to perform the changes).
        /// </summary>
        private readonly Transform _enemyTransform;


        /// <summary>
        /// The actual State of the machine.
        /// </summary>
        private State _actualState;


        private float Speed {
            get { return _speed; }
        }

        private Vector3 FirstStop {
            get { return _firstStop; }
        }

        private Vector3 SecondStop {
            get { return _secondStop; }
        }

        private Vector3 FinalPoint {
            get { return _finalPoint; }
        }


        private Transform PlayerTransform {
            get { return _playerTransform; }
        }

        private Transform EnemyTransform {
            get { return _enemyTransform; }
        }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="speed">Indicates the speed (velocity module) for this enemy.</param>
        /// <param name="firstStop">The point in which this enemy will start performing the circular movement.</param>
        /// <param name="secondStop">The center of the circle.</param>
        /// <param name="finalPoint">The final point for the enemy (i.e place in the enemy grid).</param>
        /// <param name="playerTransform">A refenrece to the player's transform (allows to get actual position).</param>
        /// <param name="enemyTransform">A reference to the enemy trasnform (in order to modify its position).</param>
        public MovementStateMachine(float speed,
            Vector3 firstStop, Vector3 secondStop, Vector3 finalPoint,
            Transform playerTransform, Transform enemyTransform) {
            _speed = speed;
            _firstStop = firstStop;
            _secondStop = secondStop;
            _finalPoint = finalPoint;
            _playerTransform = playerTransform;
            _enemyTransform = enemyTransform;
            _actualState = new MoveToFirstStop(this);
        }

        /// <summary>
        /// Executes the state machine.
        /// </summary>
        public void Execute() {
            _actualState.Move(); // First, move the enemy.
            _actualState = _actualState.NextState(); // Then, update state.
        }

        /// <summary>
        /// An abstract State.
        /// </summary>
        private abstract class State {
            /// <summary>
            /// A reference to the state machine owning this state.
            /// Can be used to access values needed by each State implementation.
            /// </summary>
            private readonly MovementStateMachine _movementStateMachine;

            /// <summary>
            /// A reference to the state machine owning this state.
            /// Can be used to access values needed by each State implementation.
            /// </summary>
            protected MovementStateMachine MovementStateMachine {
                get { return _movementStateMachine; }
            }

            protected State(MovementStateMachine movementStateMachine) {
                _movementStateMachine = movementStateMachine;
            }


            /// <summary>
            /// Moves the element whose transform is the given one.
            /// </summary>
            public abstract void Move();

            /// <summary>
            /// Returns the next state.
            /// </summary>
            /// <returns>The next state.</returns>
            public abstract State NextState();
        }

        /// <summary>
        /// Represents a linea movement.
        /// </summary>
        private abstract class LinealMovement : State {
            /// <summary>
            /// The target towards which the enemy will move to.
            /// </summary>
            private readonly Vector3 _target;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="movementStateMachine">A reference to the state machine owning this state.</param>
            /// <param name="target">The target towards which the enemy will move to.</param>
            protected LinealMovement(MovementStateMachine movementStateMachine, Vector3 target)
                : base(movementStateMachine) {
                _target = target;
            }

            /// <summary>
            /// Moves the enemy.
            /// </summary>
            public override void Move() {
                var actual = MovementStateMachine.EnemyTransform.position;
                var maxDistanceMoved = MovementStateMachine.Speed * Time.deltaTime;
                MovementStateMachine.EnemyTransform.position = Vector3.MoveTowards(actual, _target, maxDistanceMoved);
            }

            /// <summary>
            /// Returns the next state.
            /// </summary>
            /// <returns>The next state.</returns>
            public override State NextState() {
                var actual = MovementStateMachine.EnemyTransform.position;
                return actual.Equals(_target) ? GetNextState() : this;
            }

            /// <summary>
            /// Abstract method that allows each extension of this class define how the next state is created.
            /// </summary>
            /// <returns>The next state.</returns>
            protected abstract State GetNextState();
        }

        /// <summary>
        /// Represents a movement towards the first point.
        /// </summary>
        private class MoveToFirstStop : LinealMovement {
            public MoveToFirstStop(MovementStateMachine movementStateMachine) :
                base(movementStateMachine, movementStateMachine.FirstStop) {
            }

            protected override State GetNextState() {
                return new MoveToSecondStop(MovementStateMachine);
            }
        }

        /// <summary>
        /// Represents a movement towards the second point.
        /// </summary>
        private class MoveToSecondStop : LinealMovement {
            public MoveToSecondStop(MovementStateMachine movementStateMachine) :
                base(movementStateMachine, movementStateMachine.SecondStop) {
            }

            protected override State GetNextState() {
                return new MoveToFinalPoint(MovementStateMachine);
            }
        }

        /// <summary>
        /// Represents a movement towards the final point.
        /// </summary>
        private class MoveToFinalPoint : LinealMovement {
            private readonly WaitAttack _waitAttack;

            public MoveToFinalPoint(MovementStateMachine movementStateMachine) :
                base(movementStateMachine, movementStateMachine.FinalPoint) {
                _waitAttack = new WaitAttack(movementStateMachine, this);
            }

            protected override State GetNextState() {
                _waitAttack.Restart(); // First, restart the state.
                return _waitAttack;
            }
        }

        private class WaitAttack : State {
            private float _attackWaitTime;

            private readonly MoveToFinalPoint _previousMoveToFinalPointState;

            public WaitAttack(MovementStateMachine movementStateMachine, MoveToFinalPoint previousMoveToFinalPointState)
                : base(movementStateMachine) {
                _previousMoveToFinalPointState = previousMoveToFinalPointState;
                Restart();
            }

            public override void Move() {
                // Does not move, only update the wait time.
                _attackWaitTime -= Time.deltaTime;
            }

            public override State NextState() {
                if (_attackWaitTime <= 0) {
                    return new AttackPlayer(MovementStateMachine, _previousMoveToFinalPointState);
                }
                return this;
            }

            internal void Restart() {
                _attackWaitTime = Random.Range(1.0f, 3.0f);
            }
        }

        private class AttackPlayer : LinealMovement {
            private readonly MoveToFinalPoint _previousMoveToFinalPointState;

            public AttackPlayer(MovementStateMachine movementStateMachine,
                MoveToFinalPoint previousMoveToFinalPointState) :
                base(movementStateMachine, movementStateMachine.PlayerTransform.position) {
                _previousMoveToFinalPointState = previousMoveToFinalPointState;
            }

            protected override State GetNextState() {
                return _previousMoveToFinalPointState;
            }
        }
    }
}