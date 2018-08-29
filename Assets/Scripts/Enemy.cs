using System;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public Rigidbody2D body;
    public BoxCollider2D collider;

    [SerializeField] private float speed;

    private MovementStateMachine _movementStateMachine;

    public Enemy() {
        _movementStateMachine = null;
    }


    // Use this for initialization
    private void Start() {
    }

    // Update is called once per frame
    private void Update() {
        if (_movementStateMachine != null) {
            _movementStateMachine.Execute();
        }
    }

    public void Spawn(Vector3 startingPoint, Vector3 circlePoint, Vector3 finalPoint) {
        if (_movementStateMachine != null) {
            return;
        }
        transform.position = startingPoint;
        _movementStateMachine = new MovementStateMachine(speed, circlePoint, finalPoint, transform);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        Destroy(gameObject);
        _movementStateMachine = null;
    }

    /// <summary>
    /// The state machine that dictates how the enemy moves.
    /// </summary>
    private class MovementStateMachine {
        private readonly Vector3 _finalPoint;

        private State _actualState;

        private readonly Transform _enemyTransform;

        public MovementStateMachine(float speed, Vector3 circlePoint, Vector3 finalPoint, Transform enemyTransform) {
//            _startingPoint = startingPoint;
            _enemyTransform = enemyTransform;
            _actualState = new MoveToCircleState(speed, circlePoint, finalPoint);
        }

        public void Execute() {
            _actualState.Move(_enemyTransform); // First, move the enemy.
            _actualState = _actualState.NextState(); // Then, update state.
        }


        private abstract class State {
            private readonly float _speed;


            private readonly Vector3 _circlePoint;

            private readonly Vector3 _finalPoint;

            private readonly Transform _enemyTransform;


            protected float Speed {
                get { return _speed; }
            }

            protected Vector3 CirclePoint {
                get { return _circlePoint; }
            }

            protected Vector3 FinalPoint {
                get { return _finalPoint; }
            }

            protected Transform EnemyTransform {
                get { return _enemyTransform; }
            }


            protected State(float speed, Vector3 circlePoint, Vector3 finalPoint, Transform enemyTransform) {
                _circlePoint = circlePoint;
                _finalPoint = finalPoint;
                _enemyTransform = enemyTransform;
            }


            /// <summary>
            /// Moves the element whose transform is the given one.
            /// </summary>
            /// <param name="transform">The transform whose position must be modified.</param>
            public abstract void Move(Transform transform);

            /// <summary>
            /// Returns the next state.
            /// </summary>
            /// <returns>The next state.</returns>
            public abstract State NextState();
        }


        private class MoveToCircleState : State {
            public MoveToCircleState(float speed, Vector3 circlePoint, Vector3 finalPoint, Transform enemyTransform) :
                base(speed, circlePoint, finalPoint, enemyTransform) {
            }

            public override void Move(Transform transform) {
                transform.position = new Vector3(0, 0, 0);
                transform.position = Vector3.MoveTowards(transform.position, CirclePoint, Time.deltaTime * Speed);
            }

            public override State NextState() {
                return EnemyTransform.position.Equals(CirclePoint) // TODO: equals or less than an epsilon?
                    ? new MoveToCircleState(Speed, CirclePoint, FinalPoint, EnemyTransform)
                    : this;
            }
        }

        private class PerformCircle : State {
            public PerformCircle(float speed, Vector3 circlePoint, Vector3 finalPoint, Transform enemyTransform) :
                base(speed, circlePoint, finalPoint, enemyTransform) {
            }

            public override void Move(Transform transform) {
                throw new NotImplementedException();
            }

            public override State NextState() {
                throw new NotImplementedException();
            }
        }
    }
}